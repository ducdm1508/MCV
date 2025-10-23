import axios from "axios";
const BASE_URL = "https://localhost:7275/api";

export const loginUser = async (username, password) => {
  const response = await axios.post(`${BASE_URL}/Auth/login`, {
    username,
    password,
    
  },{ withCredentials: true });
  return response.data;
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
