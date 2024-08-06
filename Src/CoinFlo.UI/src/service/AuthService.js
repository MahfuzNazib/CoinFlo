import axios from 'axios';
import router from '../router/index';
import { BASE_API_URL } from '../config'; 

export async function refreshToken() {
    const token = localStorage.getItem('authToken');
    const refreshToken = localStorage.getItem('refreshToken');

    if (!token || !refreshToken) {
        return;
    }

    try {
        const response = await axios.post(`${BASE_API_URL}Auth/RefreshToken`, {
            token: token,
            refreshToken: refreshToken
        });

        if (response.data.status) {
            localStorage.setItem('authToken', response.data.data.token);
            localStorage.setItem('refreshToken', response.data.data.refreshToken);
        } else {
            router.push({ path: '/auth/login' });
        }
    } catch (error) {
        router.push({ path: '/auth/login' });
    }
}
