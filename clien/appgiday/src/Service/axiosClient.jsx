import axios from "axios";

const BASE_URL = "https://localhost:7275/api";

const api = axios.create({
  baseURL: BASE_URL,
  withCredentials: true,
});

let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  
  isRefreshing = false;
  failedQueue = [];
};

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

    if (!error.response || error.response.status !== 401 || originalRequest._retry) {
      return Promise.reject(error);
    }

    if (isRefreshing) {
      return new Promise((resolve, reject) => {
        failedQueue.push({ resolve, reject });
      })
        .then(token => {
          originalRequest.headers["Authorization"] = `Bearer ${token}`;
          return api(originalRequest);
        })
        .catch(err => Promise.reject(err));
    }

    originalRequest._retry = true;
    isRefreshing = true;

    try {
      const refreshRes = await axios.post(
        `${BASE_URL}/Auth/refresh`,
        {},
        { withCredentials: true }
      );

      const newAccessToken = refreshRes.data.accessToken;
      localStorage.setItem("token", newAccessToken);

      originalRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;
      
     
      processQueue(null, newAccessToken);
      
      return api(originalRequest);
    } catch (refreshError) {
      console.error("Refresh token hết hạn hoặc không hợp lệ:", refreshError);
      
      localStorage.removeItem("token");
      processQueue(refreshError, null);
      
      
      window.location.href = "/login";
      
      return Promise.reject(refreshError);
    }
  }
);

export default api;