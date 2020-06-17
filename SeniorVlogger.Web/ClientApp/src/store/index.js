import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export const store = new Vuex.Store({
    state: {
        user: {
            name: '',
            isEmailConfirmed: false,
            isSubscribed: false,
            token: ''
        }
    },
    getters: {
        AUTH: state => {
            return (state.user.token || localStorage.getItem('token')) ? true : false
        },

        TOKEN: state => {
            return state.user.token || localStorage.getItem('token')
        }
    },
    mutations: {
        SET_USER: (state, payload) => {
            state.user.name = payload.user
            state.user.isEmailConfirmed = payload.isEmailConfirmed
            state.user.isSubscribed = payload.isSubscribed
            state.user.token = payload.token
            if(payload.remember)
                localStorage.setItem('token', payload.token)
        },

        DELETE_USER: (state, payload) => {
            state.user.name = ''
            state.user.isEmailConfirmed = false
            state.user.isSubscribed = false
            state.user.token = ''
            localStorage.removeItem('token')
        }
    },
    actions: {
        LOGIN: async (context, payload) => {
            let { data } = await axios.post('/api/User/Login', payload)
            if (data.user) {
                if(payload.remember)
                    data.remember = true
                context.commit('SET_USER', data)
            }
            return data
        },

        LOGOUT: async (context, payload) => {
            let { data } = await axios.post('/api/User/Logout')
            context.commit('DELETE_USER')
        }
    }
})

// //add new todo item
// let item = 'New TODO'
// this.$store.dispatch('SAVE_TODO', item)

// //get elements and put in todos
// mounted() {
//     this.$store.dispatch('GET_TODO')
// }

// //get todolist
// let todolist = this.$store.getters.TODOS