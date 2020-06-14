export default function auth ({next, store, nextMiddleware}){
    if(!store.getters.AUTH.loggedIn){
        return next({
            name: 'login'
        })
    }
    return nextMiddleware()
}