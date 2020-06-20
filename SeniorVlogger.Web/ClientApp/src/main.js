import Vue from 'vue'
import axios from 'axios'
import { store } from './store'
import App from './App.vue'
import router from './router'
import './assets/bootstrap.min.css'
import './assets/style.sass'

import Default from "./layouts/Default.vue"
import About from "./layouts/About.vue"
import Blog from "./layouts/Blog.vue"
import MainSlide from "./layouts/MainSlide.vue"
import Manage from "./layouts/Manage.vue"

Vue.component('default-layout', Default)
Vue.component('about-layout', About)
Vue.component('blog-layout', Blog)
Vue.component('mainslide-layout', MainSlide)
Vue.component('manage-layout', Manage)

axios.interceptors.request.use(config => {
    let token = store.getters.TOKEN
    if (token) {
        config.headers.Authorization = 'Bearer ' + token
    }

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
