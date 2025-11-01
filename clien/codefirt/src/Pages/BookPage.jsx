import React, { useState, useEffect } from "react";
import axios from "axios";
import BookForm from "../Components/Book/BookForm";
import BookList from "../Components/Book/BookList";

const BookPage = () => {
  const [books, setBooks] = useState([]);
  const [editingBook, setEditingBook] = useState(null);

  const fetchBooks = async () => {
    try {
      const res = await axios.get("https://localhost:7111/api/Book");
      setBooks(res.data);
    } catch (error) {
      console.error("Lỗi khi lấy danh sách sách:", error);
    }
  };

  useEffect(() => {
    fetchBooks();
  }, []);

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa sách này?")) {
      try {
        await axios.delete(`https://localhost:7111/api/Book/${id}`);
        fetchBooks();
      } catch (error) {
        console.error("Lỗi khi xóa sách:", error);
      }
    }
  };

  const handleEdit = (book) => {
    setEditingBook(book);
  };

  const handleCancelEdit = () => {
    setEditingBook(null);
  };

  return (
    <div>
      <h1>Danh Sách Sách</h1>

      <BookForm
        editingBook={editingBook}
        onBookUpdated={fetchBooks}
        onCanEdit={handleCancelEdit}
      />

      <div style={{ display: "flex", flexWrap: "wrap" }}>
        {books.map(book => (
          <BookList
            key={book.id}
            book={book}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        ))}
      </div>
    </div>
  );
};

export default BookPage;
