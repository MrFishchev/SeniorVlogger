import Vue from 'vue'
import VueRouter from 'vue-router'
import {store} from '../store'
import Home from '../views/Home.vue'

import middlewarePipeline from './middlewarePipeline'
import guest from './middleware/guest'
import auth from './middleware/auth'
import isSubscribed from './middleware/isSubscribed'

Vue.use(VueRouter)

  const routes = [
  {
    path: '/login',
    name: 'login',
    meta: {layout: 'mainslide', middleware: [ guest ]},
    component: function () {
      return import('../views/Login.vue')
    }
  },

  {
    path: '/',
    name: 'Home',
    meta: { layout: 'mainslide'},
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    meta: { layout: 'default'},
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
    meta: { layout: 'manage'},
    component: function () {
      return import(/* webpackChunkName: "manage" */ '../views/manage/Index.vue')
    },

    children: [
      {
        path: 'posts',
        component: function () {
          return import('../views/manage/posts/Index.vue')
        },
      },

      {
        path: 'posts/create',
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

router.beforeEach((to, from, next) => {
  if(!to.meta.middleware) {
    return next()
  }
  const middleware = to.meta.middleware
  const context = {
    to, from, next, store
  }

  return middleware[0] ({
    ...context,
    nextMiddleware: middlewarePipeline(context, middleware, 1)
  })
}) 

export default router
