import { BASE_API_URL } from './config';

export default {
    install(app) {
        app.config.globalProperties.$BASE_API_URL = BASE_API_URL;
    }
};
