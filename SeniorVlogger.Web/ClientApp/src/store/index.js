import Vue from 'vue'
import Vuex from 'vuex'
import createPersistedState from 'vuex-persistedstate'
import axios from 'axios'
import SecureLS from "secure-ls";
const ls = new SecureLS({ isCompression: false });

Vue.use(Vuex)

export const store = new Vuex.Store({
    plugins: [
        createPersistedState({
            storage: {
                getItem: key => ls.get(key),
                setItem: (key, value) => ls.set(key, value),
                removeItem: key => ls.remove(key)
            }
        })
    ],
    state: {
        user: {
            name: null,
            isEmailConfirmed: false,
            isSubscribed: false,
            token: null
        }
    },
    getters: {
        AUTH: state => {
            return state.user.token ? true : false
        },

        TOKEN: state => {
            return state.user.token
        },

        USER: state => {
            return state.user
        }
    },
    mutations: {
        SET_USER: (state, payload) => {
            state.user.name = payload.user
            state.user.isEmailConfirmed = payload.isEmailConfirmed
            state.user.isSubscribed = payload.isSubscribed
            state.user.token = payload.token
        },

        DELETE_USER: (state, payload) => {
            state.user.name = null
            state.user.isEmailConfirmed = false
            state.user.isSubscribed = false
            state.user.token = null
        }
    },
    actions: {
        LOGIN: async (context, payload) => {
            let { data } = await axios.post('/api/User/Login', payload)
            if (data.user) {
                data.remember = payload.remember ? true : false
                context.commit('SET_USER', data)
            }
            return data
        },

        LOGOUT: async (context, payload) => {
            let { data } = await axios.post('/api/User/Logout')
            context.commit('DELETE_USER')
        },

        VERIFY: async (context, payload) => {
            let { data } = await axios.get('/api/User/Verify')
            if (data.user) {
                context.commit('SET_USER', data)
            }
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