import Vue from 'vue'
import axios from 'axios'
import {store} from './store'
import App from './App.vue'
import router from './router'
import 'bootstrap/dist/css/bootstrap.min.css'

import Default from "./layouts/Default.vue"
import Blog from "./layouts/Blog.vue"
import MainSlide from "./layouts/MainSlide.vue"
import Manage from "./layouts/Manage.vue"

Vue.component('default-layout', Default)
Vue.component('blog-layout', Blog)
Vue.component('mainslide-layout', MainSlide)
Vue.component('manage-layout', Manage)

axios.interceptors.request.use(config => {
    let token = store.getters.TOKEN
    if (token) {
        config.headers.Authorization = 'Bearer ' + token
    }
    console.log(config)

    return config
}, (error) => {
    return Promise.reject(error)
})
Vue.prototype.$api = axios


Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: function (h) { return h(App) }
}).$mount('#app')
