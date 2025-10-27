import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import LoginPage from "./Pages/LoginPage";
import HomePage from "./Pages/HomePage";
import "./App.css";
import ListUserPage from "./Pages/Admin/ListUserPage";

function App() {
  return (
    <Router>
      <div className="App">
        {/* Menu điều hướng */}
        <nav>
          <Link to="/">Home</Link> |{" "}
          <Link to="/login">Login</Link>
        </nav>

        {/* Các route tương ứng */}
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/listuser" element={<ListUserPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
