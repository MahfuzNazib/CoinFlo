export class CategoryService {
    getAllCategory() {
        return fetch('/demo/data/category.json', { headers: { 'Cache-Control': 'no-cache' } })
            .then((res) => res.json())
            .then((d) => d.data);
    }
}
