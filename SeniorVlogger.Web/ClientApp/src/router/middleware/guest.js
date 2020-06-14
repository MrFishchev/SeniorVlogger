export default function guest({ next, store, nextMiddleware }) {
    if(store.getters.AUTH){
        return next({
            name: 'Manage'
        })
    }
    return nextMiddleware()
}