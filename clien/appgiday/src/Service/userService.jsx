
import api from "./axiosClient";


export const loginUser = async (username, password) => {
  const response = await api.post("/Auth/login", {
    username,
    password,
    
  },{ withCredentials: true });
  return response.data;
};

export const logoutUser = async () => {
  try {
   
    localStorage.removeItem("token");
  
    await api.post(
      "/Auth/logout",
      {},
      { withCredentials: true }
    );
  } catch (error) {
    console.error("Lỗi đăng xuất:", error.message);
    
  }
};

export function getRoleFromToken() {
  const token = localStorage.getItem("token");
  if (!token) return null;
  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  } catch (e) {
    return null;
  }
}

export const getAllUsers = async () => {
  const response = await api.get("/User");
  return response.data;
} 
