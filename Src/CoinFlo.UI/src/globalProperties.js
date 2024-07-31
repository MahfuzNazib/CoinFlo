export default {
    install(app) {
        app.config.globalProperties.$BASE_API_URL = 'http://localhost:7092/api/';
    }
};
