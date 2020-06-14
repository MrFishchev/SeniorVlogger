export default function auth({ next, store, nextMiddleware }) {
    if(!store.getters.AUTH){
        return next({
            name: 'login'
        })
    }
    return nextMiddleware()
}