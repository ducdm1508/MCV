import axios from "axios";

const BASE_URL = "https://localhost:7275/api";

const api = axios.create({
  baseURL: BASE_URL,
  withCredentials: true 
});


api.interceptors.request.use(config => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
});


api.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;

    if (error.response && error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const refreshRes = await axios.post(`${BASE_URL}/Auth/refresh`, {}, { withCredentials: true });
        localStorage.setItem("token", refreshRes.data.accessToken);
        originalRequest.headers["Authorization"] = `Bearer ${refreshRes.data.accessToken}`;
        return api(originalRequest);
      } catch (err) {
        localStorage.removeItem("token");
        window.location.href = "/login";
        return Promise.reject(err);
      }
    }
    return Promise.reject(error);
  }
);

export default api;
