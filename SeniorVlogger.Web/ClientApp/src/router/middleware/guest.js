export default function guset ({next, store, nextMiddleware}){
    if(store.getters.AUTH.loggedIn){
        return next({
            name: 'manage'
        })
    }
    return nextMiddleware()
}