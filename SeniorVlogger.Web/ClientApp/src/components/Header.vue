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
  
      <div class="manage" v-if="authorized">
        <router-link to="/manage">Manage</router-link>
        <a @click.prevent="logout">Logout</a>
      </div>
    </div>
  </div>
</template>

<style lang="sass">
    @import '@/assets/_colors'
    #header
      height: 50px
      background: $base-dark
      border-bottom: 3px solid $neon-light
      position: fixed
      top: 0
      width: 100vw
      z-index: 100
      display: flex
      align-items: center
      a
        color: #fff
      &.blog
        background: #fff
        a
          color: #303030
        .container .manage 
          a
           color: #303030
          &:hover
            color: $neon-light
      .container
        display: flex
        justify-content: space-between
        max-width: 730px
        padding: 0
        nav
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
                    color: $neon-light
                  &:hover
                    color: $neon-light
              li:first-child
                margin: 0
        .manage
          color: $font-white
          a
            color: $font-white
            text-decoration: none
            transition: color 0.3s
            &:first-child
              margin-right: 1vw
            &:hover
              color: $neon-light
</style>

<script>
export default {
  computed: {
    authorized() {
        return this.$store.getters.AUTH
    }
  },
  methods: {
    async logout () {
      await this.$store.dispatch('LOGOUT')
      this.$router.replace('/')
    }
  }
}
</script>
