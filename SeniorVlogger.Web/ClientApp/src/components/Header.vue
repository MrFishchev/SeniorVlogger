<template>
  <div class="wrapper" id="header">
    <div class="container">
      <nav>
        <div class="container">
          <ul>
            <li><router-link to="/">Home</router-link></li>
            <li><router-link to="/blog" active-class="active">Blog</router-link></li>
            <li><router-link to="/about" active-class="active">About</router-link></li>
          </ul>
        </div>
      </nav>
  
      <div class="manage">
        <div class="d-inline-block" v-if="isBlogLayout">
          <i v-if="isLightMode" @click="changeLightMode('dark')" class="light-mode far fa-moon"></i>
          <i v-else @click="changeLightMode('light')" class="light-mode fas fa-sun"></i>
        </div>
        <router-link v-if="authorized" to="/manage">Manage</router-link>
        <a v-if="authorized" @click.prevent="logout">Logout</a>
      </div>
    </div>
  </div>
</template>

<style lang="sass">
    @import '@/assets/_colors'
    #header
      min-width: 320px
      height: 50px
      background: var(--base-dark)
      border-bottom: 3px solid var(--neon-light)
      position: fixed
      top: 0
      width: 100vw
      z-index: 100
      display: flex
      align-items: center
      a
        color: #fff
      .container
        display: flex
        justify-content: space-between
        max-width: 1080px
        padding: 0
        nav
          display: flex
          flex-direction: column
          justify-content: space-evenly
          .container
            align-items: center
            display: flex
            padding: 0
            ul
              width: 100%
              margin: 0
              padding: 0
              li
                display: inline-block
                margin-left: 2vw
                a
                  text-decoration: none
                  font-size: 1rem
                  text-transform: uppercase
                  cursor: pointer
                  transition: color 0.5s
                  &.exact-active-route, &.active
                    color: var(--neon-light)
                  &:hover
                    color: var(--neon-light)
              li:first-child
                margin: 0
        .manage
          color: var(--font-white)
          a
            color: var(--font-white)
            text-decoration: none
            transition: color 0.3s
            margin-left: 2vw
            &:first-child
              margin-right: 1vw
            &:hover
              color: var(--neon-light)
          .light-mode
            font-size: 1.563rem
            color: var(--font-white)
            &:hover
              color: var(--neon-light)
              cursor: pointer
              transition: color 0.3s
</style>

<script>
export default {
  props:{
    isLightMode: Boolean
  },
  
  computed: {
    authorized() {
        return this.$store.getters.AUTH
    },
    isBlogLayout(){
      return this.$route.meta.layout === 'blog'
    }
  },
  
  methods: {
    async logout () {
      await this.$store.dispatch('LOGOUT')
      this.$router.replace('/')
    },
    
    changeLightMode(value){
      this.$emit('light-mode-changed', value === 'light')
    }
  }
}
</script>
