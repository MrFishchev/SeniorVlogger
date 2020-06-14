import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

  const routes = [
  {
    path: '/',
    name: 'Home',
    meta: { layout: "mainslide"},
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    meta: { layout: "default"},
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: function () {
      return import(/* webpackChunkName: "about" */ '../views/About.vue')
    }
  },
  
  //MANAGE SECTION
  {
    path: '/manage',
    name: 'Manage',
    meta: { layout: "manage"},
    component: function () {
      return import(/* webpackChunkName: "manage" */ '../views/manage/Index.vue')
    },

    children: [
      {
        path: 'posts',
        component: function () {
          return import('../views/manage/posts/Index.vue')
        }
      },
      {
        path: 'create',
        component: function () {
          return import(/* webpackChunkName: "createpost" */ '../views/manage/posts/Create.vue')
        }
      }
    ]
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
