import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5265/api", // 👈 your .NET API base URL
});

// Automatically attach token if logged in
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");

  if (!config.headers) {
    config.headers = {};
  }

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});


export default api;
