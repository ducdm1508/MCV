import axios from 'axios';
import React, { useEffect, useState } from 'react';

const BookForm = ({ onBookUpdated, editingBook, onCanEdit }) => {
  const [formData, setFormData] = useState({
    title: "",
    publicationYear: "",
    authorId: "",
  });
  const [message, setMessage] = useState("");
  const [authors, setAuthors] = useState([]);

    useEffect(() => {
    const fetchAuthors = async () => {
      try {
        const res = await axios.get("https://localhost:7111/api/Author"); 
        setAuthors(res.data);
      } catch (err) {
        console.error("Lỗi khi load authors:", err);
      }
    };
    fetchAuthors();
  }, []);
  useEffect(() => {
    if (editingBook) {
      setFormData({
        title: editingBook.title,
        publicationYear: editingBook.publicationYear,
        authorId: editingBook.authorId,
      });
    } else {
      setFormData({
        title: "",
        publicationYear: "",
        authorId: "",
      });
    }
  }, [editingBook]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingBook && editingBook.id) {
        await axios.put(`https://localhost:7111/api/Book/${editingBook.id}`, formData);
        setMessage("Cập nhật sách thành công!");
        onCanEdit();
      } else {
        await axios.post("http://localhost:7111/api/Book", formData);
        setMessage("Thêm sách thành công!");
      }
      setFormData({ title: "", publicationYear: "", authorId: "" });
      onBookUpdated();
    } catch (error) {
      console.error(error);
      setMessage("Đã xảy ra lỗi. Vui lòng thử lại.");
    }
  }

  return (
    <div>
      <h2>{editingBook ? "Cập nhật Sách" : "Thêm Sách Mới"}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Tiêu đề:</label>
          <input type='text' name='title' value={formData.title} onChange={handleChange} required />
        </div>
        <div>
          <label>Năm xuất bản:</label>
          <input type='number' name='publicationYear' value={formData.publicationYear} onChange={handleChange} required />
        </div>
        <div>
          <label>ID Tác giả:</label>
          <select name="authorId" value={formData.authorId} onChange={handleChange}>
            {authors.map(a => (
              <option key={a.id} value={a.id}>{a.name}</option>
            ))}
          </select>
        </div>
        {message && <p>{message}</p>}
        <button type='submit'>{editingBook ? "Cập nhật" : "Thêm"}</button>
        {editingBook && <button type='button' onClick={onCanEdit} style={{ marginLeft: "10px" }}>Hủy</button>}
      </form>
    </div>
  );
};

export default BookForm;
