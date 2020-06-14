import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export const store = new Vuex.Store({
    state: {
        todos: null
    },
    getters: {
        TODOS: state => {
            return state.todos
        }
    },
    mutations: {
        SET_TODO: (state, payload) => {
            state.todos = payload
        },

        ADD_TODO: (state, payload) => {
            state.todos.push(payload)
        }
    },
    actions: {
        GET_TODO: async (context, payload) => {
            let {data} = await axios.get('url')
            context.commit('SET_TODO', data)
        },

        SAVE_TODO: async (context, payload) => {
            let {data} = await axios.post('url')
            context.commit('ADD_TODO', payload)
        }
    }
})


// //add new todo item
// let item = 'New TODO'
// this.$store.dispath('SAVE_TODO', item)

// //get elements and put in todos
// mounted() {
//     this.$store.dispath('GET_TODO')
// }

// //get todolist
// let todolist = this.$store.getters.TODOS