import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export const store = new Vuex.Store({
    state: {
        user: {
            name: '',
            loggedIn: false,
            isEmailConfirmed: false,
            isSubscribed: false
        }
    },
    getters: {
        AUTH: state=> {
            return state.user.loggedIn
        }
    },
    mutations: {
        SET_USER: (state, payload) => {
            state.user.name = payload.user
            state.user.loggedIn = true
            state.user.isEmailConfirmed = payload.isEmailConfirmed
            state.user.isSubscribed = payload.isSubscribed
        },

        DELETE_USER: (state, payload) => {
            state.user.name = ''
            state.user.loggedIn = false
            state.user.isEmailConfirmed = false
            state.user.isSubscribed = false
        }
    },
    actions: {
        LOGIN: async (context, payload) => {
            let { data } = await axios.post('/Identity/User/Login', payload)
            if (data.user) {
                context.commit('SET_USER', data)
            }
            return data
        },

        LOGOUT: async (context, payload) => {
            let { data } = await axios.post('/Identity/User/Logout')
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