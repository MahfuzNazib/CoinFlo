export default {
    install(app) {
        app.config.globalProperties.$BASE_API_URL = 'https://localhost:7092/api/';
    }
};
