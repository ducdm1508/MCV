import React, { useEffect, useState } from 'react';
import axios from 'axios';

const Department = () => {
    const [dpm, setDpm] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [searchTerm, setSearchTerm] = useState("");
    const [searchInput, setSearchInput] = useState("");
    const [totalPages, setTotalPages] = useState(0);
    const [isEditing, setIsEditing] = useState(false);

    const [form, setForm] = useState({
        departmentName: "",
        dean: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    }

    const handleReset = () => {
        setForm({
            departmentId: "",
            departmentName: "",
            dean: "",
        });
        setIsEditing(false);
    }

    const handleEdit = (department) => {
        setForm({
            departmentId: department.departmentId,
            departmentName: department.departmentName,
            dean: department.dean,
        });
        setIsEditing(true);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (isEditing) {
                await axios.put(`https://localhost:7293/api/Departments/${form.departmentId}`, form);
                setIsEditing(false);
            } else {
                await axios.post("https://localhost:7293/api/Departments", form);
            }
            fetchDepartments();
        } catch (error) {
            console.error(error);
        }
    };

    const handleDelete = async (departmentId) => {
        if (!window.confirm("Bạn có chắc muốn xoá khoa này?")) return;
        try {
            await axios.delete(`https://localhost:7293/api/Departments/${departmentId}`);
            fetchDepartments();
        } catch (error) {
            console.error(error);
        }
    };


    const fetchDepartments = async () => {
        try {
            const res = await axios.get("https://localhost:7293/api/Departments", {
                params: {
                    pageNum: pageNumber,
                    pageSize: pageSize,
                    search: searchTerm
                }
            });
            console.log("Dữ liệu trả về:", res.data);

            setDpm(res.data.data);
            setTotalPages(res.data.totalPage);
        }
        catch (error) {
            console.error("Lỗi khi lấy danh sách phòng ban:", error);
        }
    };

    const handleSearchChange = (e) => {
        setSearchInput(e.target.value);
    };

    const handleSearchSubmit = (e) => {
        e.preventDefault();
        setSearchTerm(searchInput);
        setPageNumber(1);
    };

    useEffect(() => {
        fetchDepartments();
    }, [searchTerm, pageNumber, pageSize]);

    return (
        <div>
            <form onSubmit={handleSearchSubmit}>
                <input
                    type="text"
                    placeholder='tìm kiếm...'
                    value={searchInput}
                    onChange={handleSearchChange}
                />
                <button type='submit'>Tìm kiếm</button>
            </form>

            <form action="" onSubmit={handleSubmit}>
                <div>
                    <label>DepartmentName:</label>
                    <input type="text" name='departmentName' onChange={handleChange} value={form.departmentName} placeholder='departname' required />
                </div>
                <div>
                    <label>Dean:</label>
                    <input type="text" name='dean' onChange={handleChange} value={form.dean} placeholder='dean' required />
                </div>
                <div>
                    <button type="submit">{isEditing ? "Cập nhật" : "Thêm mới"}</button>
                    {isEditing && <button type="button" onClick={handleReset}>Hủy</button>}
                </div>

            </form>

            <table>
                <thead>
                    <tr>
                        <th>DepartmentName</th>
                        <th>Dean</th>
                        <th>action</th>
                    </tr>
                </thead>
                <tbody>
                    {dpm.map(department => (
                        <tr key={department.departmentId}>
                            <td>{department.departmentName}</td>
                            <td>{department.dean}</td>
                            <td>
                                <button onClick={() => handleEdit(department)}>Sửa</button>
                                <button onClick={() => handleDelete(department.departmentId)}>Xoá</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <div>
                <button
                    disabled={pageNumber === 1}
                    onClick={() => setPageNumber(pageNumber - 1)}
                >
                    Trước
                </button>

                {Array.from({ length: totalPages }, (_, index) => index + 1)
                    .filter(page =>
                        page >= pageNumber - 1 && page <= pageNumber + 1
                    )
                    .map(page => (
                        <button
                            key={page}
                            onClick={() => setPageNumber(page)}
                            disabled={pageNumber === page}
                        >
                            {page}
                        </button>
                    ))}
                <button
                    disabled={pageNumber >= totalPages}
                    onClick={() => setPageNumber(pageNumber + 1)}
                >
                    Sau
                </button>
            </div>
            <select value={pageSize} onChange={(e) => {
                setPageSize(Number(e.target.value));
                setPageNumber(1);
            }}>
                <option value={5}>5</option>
                <option value={10}>10</option>
                <option value={20}>20</option>
                <option value={50}>50</option>
            </select>
        </div>
    );
};

export default Department;
