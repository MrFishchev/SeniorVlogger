export default function auth({ next, store, nextMiddleware }) {
    if(!store.getters.AUTH){
        return next({
            name: 'Login'
        })
    }
    return nextMiddleware()
}