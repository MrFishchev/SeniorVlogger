import Vue from 'vue'
import axios from 'axios'
import { store } from './store'
import App from './App.vue'
import router from './router'
import './assets/bootstrap.min.css'
import './assets/style.sass'

import VueFacebook from '@/assets/scripts/fb-comments.js'
Vue.use(VueFacebook)
Vue.prototype.$fb = VueFacebook

import Loading from 'vue-loading-overlay'
import 'vue-loading-overlay/dist/vue-loading.css'
Vue.use(Loading);

import VueGoodTablePlugin from 'vue-good-table'
import 'vue-good-table/dist/vue-good-table.css'
Vue.use(VueGoodTablePlugin);

import Notifications from 'vue-notification'
Vue.use(Notifications)

import VueSweetalert2 from 'vue-sweetalert2'
import 'sweetalert2/dist/sweetalert2.css'
Vue.use(VueSweetalert2);

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

let loader 
axios.interceptors.request.use(config => {
            if(loader && loader.isActive) return
            loader = Vue.$loading.show({
                color: '#079af0',
                loader: 'bars',
                width: 128,
                height: 128,
                opacity: 0.5
            })
  
        let token = store.getters.TOKEN
        if (token) {
            config.headers.Authorization = 'Bearer ' + token
        }
        return config
    }, 
    (error) => {
    return Promise.reject(error)
})

axios.interceptors.response.use(response => { 
    if(loader && loader.isActive) loader.hide()
    return response 
}, error => {
    if(loader && loader.isActive) loader.hide()
    if(error.response.status === 401){
        store.commit('DELETE_USER')
        router.replace({ path: '/login'})
    }
    return Promise.reject(error)
})


Vue.prototype.$api = axios

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: function (h) { return h(App) }
}).$mount('#app')
