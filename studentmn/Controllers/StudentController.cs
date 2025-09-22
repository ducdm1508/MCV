using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using studentmn.Data;
using studentmn.Models;

namespace studentmn.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index(int page = 1, string search = "", string className = "", string subjectFilter = "", string scoreRange = "")
        {
            int pageSize = 20;

        
            var studentScores = await _context.StudentScores
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .ToListAsync();

            var allSubjects = await _context.Subjects.ToListAsync();
            var allStudents = await _context.Students.ToListAsync();
            var allClasses = await _context.Students.Select(s => s.ClassName).Distinct().ToListAsync();


            var result = allStudents.Select(student =>
            {
                var subjectScores = allSubjects.ToDictionary(
                    subject => subject.SubjectName,
                    subject => studentScores.FirstOrDefault(ss => ss.StudentId == student.Id && ss.SubjectId == subject.Id)?.Score ?? (decimal?)0m
                );

                decimal avg = subjectScores.Values.Average() ?? 0m;

                return new StudentScoreViewModel
                {
                    StudentId = student.Id,
                    FullName = student.FullName,
                    ClassName = student.ClassName,
                    SubjectScores = subjectScores,
                    AverageScore = avg
                };
            }).ToList();


           
            result = ApplyFilters(result, search, className, subjectFilter, scoreRange);

            var totalStudents = result.Count;
            var totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize);

            var pagedResult = result
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

         
            var dashboardData = new DashboardViewModel
            {
                TotalStudents = result.Count,
                AverageScore = CalculateOverallAverage(result),
                TopStudent = CalculateTopStudentAverage(result),
                LowScores = result.Count(s => s.AverageScore < 5.0m)
            };

            ViewBag.Subjects = allSubjects.Select(s => s.SubjectName).ToList();
            ViewBag.Dashboard = dashboardData;
            ViewBag.AllClasses = allClasses;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = search;
            ViewBag.SelectedClass = className;
            ViewBag.SelectedSubject = subjectFilter;
            ViewBag.SelectedScoreRange = scoreRange;

            return View(pagedResult);
        }

        private List<StudentScoreViewModel> ApplyFilters(List<StudentScoreViewModel> students, string search, string className, string subjectFilter, string scoreRange)
        {
            var filtered = students.AsQueryable();

         
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(s => s.FullName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

   
            if (!string.IsNullOrEmpty(className) && className != "All")
            {
                filtered = filtered.Where(s => s.ClassName == className);
            }

        
            if (!string.IsNullOrEmpty(subjectFilter) && subjectFilter != "All")
            {
                filtered = filtered.Where(s => s.SubjectScores.ContainsKey(subjectFilter));

                if (!string.IsNullOrEmpty(scoreRange))
                {
                    filtered = scoreRange switch
                    {
                        "excellent" => filtered.Where(s => s.SubjectScores[subjectFilter] >= 8.5m),
                        "good" => filtered.Where(s => s.SubjectScores[subjectFilter] >= 6.5m && s.SubjectScores[subjectFilter] < 8.5m),
                        "average" => filtered.Where(s => s.SubjectScores[subjectFilter] >= 5.0m && s.SubjectScores[subjectFilter] < 6.5m),
                        "weak" => filtered.Where(s => s.SubjectScores[subjectFilter] < 5.0m),
                        _ => filtered
                    };
                }
            }

            return filtered.ToList();
        }

        private decimal CalculateOverallAverage(List<StudentScoreViewModel> students)
        {
            if (!students.Any())
                return 0;

            return Math.Round(students.Average(s => s.AverageScore), 2);
        }

        private double CalculateTopStudentAverage(List<StudentScoreViewModel> students)
        {
            if (!students.Any())
                return 0;

            var topStudent = students.OrderByDescending(s => s.AverageScore).First();
            return (double)Math.Round(topStudent.AverageScore, 2);
        }


        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var studentScores = await _context.StudentScores
                .Include(ss => ss.Subject)
                .Where(ss => ss.StudentId == id)
                .ToListAsync();

            var allSubjects = await _context.Subjects.ToListAsync();

            var subjectScores = allSubjects.ToDictionary(
                subject => subject.SubjectName,
                subject => studentScores.FirstOrDefault(ss => ss.SubjectId == subject.Id)?.Score ?? (decimal?)0m
            );

            decimal avg = subjectScores.Values.Average() ?? 0m;

            var viewModel = new StudentScoreViewModel
            {
                StudentId = student.Id,
                FullName = student.FullName,
                ClassName = student.ClassName,
                BirthDate = student.BirthDate,
                SubjectScores = subjectScores,
                AverageScore = avg
            };

            ViewBag.Subjects = allSubjects.Select(s => s.SubjectName).ToList();

            return View(viewModel);
        }

        // GET: Student/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var allSubjects = await _context.Subjects.ToListAsync();

                var viewModel = new CreateStudentRequest
                {
                    AllSubjects = allSubjects,
                    SubjectScores = allSubjects.ToDictionary(s => s.Id, s => 0m),
                    BirthDate = DateTime.Today
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tải trang tạo học sinh.";
                return RedirectToAction(nameof(Index));
            }
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentRequest viewModel)
        {
            try
            {
                var allSubjects = await _context.Subjects.ToListAsync();
                viewModel.AllSubjects = allSubjects;

                if (!IsValidStudent(viewModel))
                {
                    return View(viewModel);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var student = new Student
                        {
                            FullName = viewModel.FullName?.Trim(),
                            BirthDate = viewModel.BirthDate,
                            ClassName = viewModel.ClassName?.Trim()
                        };

                        _context.Students.Add(student);
                        await _context.SaveChangesAsync();

                        var studentScores = new List<StudentScore>();

                        foreach (var subjectScore in viewModel.SubjectScores)
                        {
                            if (subjectScore.Value >= 0)
                            {
                                var score = new StudentScore
                                {
                                    StudentId = student.Id,
                                    SubjectId = subjectScore.Key,
                                    Score = subjectScore.Value
                                };
                                studentScores.Add(score);
                            }
                        }

                        if (studentScores.Any())
                        {
                            _context.StudentScores.AddRange(studentScores);
                            await _context.SaveChangesAsync();
                        }

                        TempData["Success"] = $"Đã thêm thành công học sinh {student.FullName} và điểm các môn.";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tạo học sinh. Vui lòng thử lại.";

                var allSubjects = await _context.Subjects.ToListAsync();
                viewModel.AllSubjects = allSubjects;

                return View(viewModel);
            }
        }

        private bool IsValidStudent(CreateStudentRequest viewModel)
        {
            var isValid = true;

            var age = DateTime.Today.Year - viewModel.BirthDate.Year;
            if (viewModel.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 6 || age > 25)
            {
                ModelState.AddModelError(nameof(viewModel.BirthDate), "Tuổi học sinh phải từ 6 đến 25.");
                isValid = false;
            }

            if (viewModel.SubjectScores != null)
            {
                foreach (var score in viewModel.SubjectScores)
                {
                    if (score.Value < 0 || score.Value > 10)
                    {
                        ModelState.AddModelError($"SubjectScores[{score.Key}]", "Điểm phải từ 0 đến 10.");
                        isValid = false;
                    }
                }
            }

            if (!string.IsNullOrEmpty(viewModel.FullName) &&
                !System.Text.RegularExpressions.Regex.IsMatch(viewModel.FullName, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                ModelState.AddModelError(nameof(viewModel.FullName), "Tên chỉ được chứa chữ cái và khoảng trắng.");
                isValid = false;
            }

            return isValid;
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var studentScores = await _context.StudentScores
                .Where(ss => ss.StudentId == id)
                .ToListAsync();

            var allSubjects = await _context.Subjects.ToListAsync();

            var subjectScores = allSubjects.ToDictionary(
                subject => subject.Id,
                subject => studentScores.FirstOrDefault(ss => ss.SubjectId == subject.Id)?.Score ?? 0
            );

            ViewData["AllSubjects"] = allSubjects;
            ViewData["SubjectScores"] = subjectScores;
            ViewData["Student"] = student;

            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dictionary<int, decimal> SubjectScores)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingScores = await _context.StudentScores
                        .Where(ss => ss.StudentId == id)
                        .ToListAsync();

                    foreach (var subjectScore in SubjectScores)
                    {
                        var existingScore = existingScores.FirstOrDefault(ss => ss.SubjectId == subjectScore.Key);

                        if (existingScore != null)
                        {
                            existingScore.Score = subjectScore.Value;
                            _context.Update(existingScore);
                        }
                        else if (subjectScore.Value > 0)
                        {
                            var newScore = new StudentScore
                            {
                                StudentId = id,
                                SubjectId = subjectScore.Key,
                                Score = subjectScore.Value
                            };
                            _context.Add(newScore);
                        }
                    }

                    var scoresToRemove = existingScores
                        .Where(ss => !SubjectScores.ContainsKey(ss.SubjectId) || SubjectScores[ss.SubjectId] == 0)
                        .ToList();

                    _context.StudentScores.RemoveRange(scoresToRemove);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }

            var allSubjects = await _context.Subjects.ToListAsync();
            ViewData["AllSubjects"] = allSubjects;
            ViewData["Student"] = student;
            return View(student);
        }
    }
}