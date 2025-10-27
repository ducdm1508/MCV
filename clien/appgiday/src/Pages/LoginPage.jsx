import React, { useState, useEffect } from "react";
import { loginUser, logoutUser } from "../Service/userService";

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      setIsLoggedIn(true);
    }
  }, []);

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const userData = await loginUser(username, password);
      setError("");
      localStorage.setItem("token", userData.accessToken);
      setIsLoggedIn(true);
      window.location.href = "/";
    } catch (err) {
      setError("Login thất bại. Kiểm tra tài khoản/mật khẩu.");
    }
  };

  const handleLogout = async () => {
    try{
        await logoutUser();
        setIsLoggedIn(false);
        window.location.reload();
    } catch (err) {
        console.error("Lỗi khi đăng xuất:", err);
    }
  };

  return (
    <div>
      <h2>{isLoggedIn ? "Bạn đã đăng nhập" : "Login"}</h2>

      {!isLoggedIn ? (
        <form onSubmit={handleLogin}>
          <div>
            <label>Username: </label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Tên đăng nhập"
            />
          </div>
          <div>
            <label>Password: </label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Mật khẩu"
            />
          </div>
          {error && <p style={{ color: "red" }}>{error}</p>}
          <button type="submit">Login</button>
        </form>
      ) : (
        <button onClick={handleLogout}>Logout</button>
      )}
    </div>
  );
};

export default LoginPage;
