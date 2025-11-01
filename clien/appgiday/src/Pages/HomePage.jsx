import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../Service/axiosClient'; 

const HomePage = () => {
  const [data, setData] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
      return;
    }

    const fetchData = async () => {
      try {
        const res = await api.get('/User/me');
        setData(res.data);
      } catch (error) {
        console.error("Lỗi khi lấy thông tin người dùng:", error);
        
      }
    };

    fetchData();
  }, [navigate]);

  return (
    <div>
      <h1>Đây là Home Page</h1>
      <p>Trang này chỉ xem được khi đã đăng nhập</p>

      {data ? (
        <div style={{ marginTop: "20px" }}>
          <h3>Tên đăng nhập: {data.username}</h3>
          <h3>Email: {data.email || "Chưa có email"}</h3>
          <h3>Vai trò: {data.role}</h3>
        </div>
      ) : (
        <p>Đang tải dữ liệu...</p>
      )}
    </div>
  );
};

export default HomePage;
