// src/axiosSetup.js
import axios from 'axios';
import { refreshToken } from './service/AuthService';

axios.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            await refreshToken();
            originalRequest.headers['Authorization'] = 'Bearer ' + localStorage.getItem('authToken');
            return axios(originalRequest);
        }
        return Promise.reject(error);
    }
);
