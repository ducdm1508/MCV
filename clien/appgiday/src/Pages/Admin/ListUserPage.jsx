import React, { useEffect, useState } from "react";
import { getAllUsers, getRoleFromToken } from "../../Service/userService";
import { useNavigate } from "react-router-dom";

const ListUserPage = () => {
    const [users, setUsers] = useState([]);
    const navigate = useNavigate();

  useEffect(() => {
    const role = getRoleFromToken();
    if (role !== "admin") {
      navigate("/login");
      return;
    }
    const fetchUsers = async () => {
      try {
        const data = await getAllUsers();
        console.log(data);
        setUsers(data);
      } catch (err) {
        console.error("Lỗi khi lấy danh sách user:", err);
      }
    };
    fetchUsers();
  }, []);

  return (
    <div>
      <h2>Danh sách người dùng</h2>
      <table border="1" cellPadding="8" style={{ borderCollapse: "collapse", width: "100%" }}>
        <thead>
          <tr>
            <th>Username</th>
            <th>Pass</th>
            <th>Email</th>
            <th>Role</th>
            <th>Token</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user, index) => (
            <tr key={index}>
              <td>{user.username}</td>
              <td>{user.password}</td>
              <td>{user.email}</td>
              <td>{user.role}</td>
              <td>{user.refreshToken}</td>
              <td>
                <button>Clear Token</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ListUserPage;
