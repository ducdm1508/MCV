import React from 'react';

const BookList = ({ book, index, onDelete, onEdit }) => {
  return (
    <div
      key={index}
      style={{
        margin: "10px",
        padding: "10px",
        border: "1px solid #ccc",
      }}
    >
      <img
        src={book.imageUrl ? book.imageUrl : "https://via.placeholder.com/120x180?text=No+Image"}
        alt={book.title}
        style={{ width: "120px", height: "180px" }}
      />
      <h3>{book.title}</h3>
      <p>Năm xuất bản: {book.publicationYear}</p>
      <p>Tác giả: {book.authorName}</p>

      <button onClick={() => onEdit(book)}>Cập nhật</button>
      <button onClick={() => onDelete(book.id)} style={{ marginLeft: "10px", color: "red" }}>
        Xóa
      </button>
    </div>
  );
};

export default BookList;
