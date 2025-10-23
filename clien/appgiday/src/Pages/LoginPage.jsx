import React, { useState } from "react";
import { loginUser } from "../Service/userService";


const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");


const handleLogin = async (e) => {
  e.preventDefault();
  try {
    const userData = await loginUser(username, password);
    setError("");
    localStorage.setItem("token", userData.accessToken);
    
    // ✅ THÊM DÒNG NÀY để redirect
    window.location.href = "/dashboard";  // hoặc page khác
    
  } catch (err) {
    setError("Login thất bại. Kiểm tra tài khoản/mật khẩu.");
  }
};

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <div>
          <label>Username: </label>
          <input
            type="text"
            value={username}
            onChange={e => setUsername(e.target.value)}
            placeholder="Tên đăng nhập"
          />
        </div>
        <div>
          <label>Password: </label>
          <input
            type="password"
            value={password}
            onChange={e => setPassword(e.target.value)}
            placeholder="Mật khẩu"
          />
        </div>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default LoginPage;
