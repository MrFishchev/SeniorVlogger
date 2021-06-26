<template>
  <div id="manage">
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
      <router-link :to="{ path: '/manage'}" exact class="navbar-brand" exact-active-class="active">Manage</router-link>
      <button
        class="navbar-toggler"
        type="button"
        data-toggle="collapse"
        data-target="#navbarColor01"
        aria-controls="navbarColor01"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" id="navbarColor01">
        <ul class="navbar-nav mr-auto">
          <li class="nav-item">
            <router-link :to="{ path: '/manage/posts'}" exact class="nav-link" active-class="active">Posts</router-link>
          </li>
          <li class="nav-item">
            <router-link :to="{ path: '/manage/categories'}" exact class="nav-link" active-class="active">Categories</router-link>
          </li>
          <li class="nav-item">
            <router-link :to="{ path: '/manage/subscribers'}" exact class="nav-link" active-class="active">Subscribers</router-link>
          </li>
          <li class="nav-item">
            <router-link :to="{ path: '/manage/users'}" exact class="nav-link" active-class="active">Users</router-link>
          </li>
        </ul>

        <ul class="navbar-nav ml-auto mr-5">
          <li class="nav-item">
            <router-link :to="{ path: '/about'}" exact class="nav-link">About</router-link>
          </li>
          <li class="nav-item">
            <router-link :to="{ path: '/blog'}" exact class="nav-link">Blog</router-link>
          </li>
          <li class="nav-item" v-if="auth">
            <a href="#" @click="Logout" class="nav-link">Logout ({{user}})</a>
          </li>
        </ul>
<!--        <form class="form-inline my-2 my-lg-0 d-none d-lg-block">-->
<!--          <input class="form-control mr-sm-2" type="text" placeholder="Search" />-->
<!--          <button class="btn btn-secondary my-2 my-sm-0" type="submit">Search</button>-->
<!--        </form>-->
      </div>
    </nav>

    <slot />
  </div>
</template>

<script>
    export default {
        mounted () {
          document.title = "SeniorVlogger - Manage"
        },

        computed: {
          auth() {
              return this.$store.getters.AUTH
          },
          user(){
            return this.$store.getters.USER.name
          }
        },

        methods: {
            Logout() {
                this.$store.dispatch("LOGOUT")
                    .then(res => this.$router.push("/login"))
            }
        }
    }
</script>

<style lang="sass" scoped>
body
  background: #fff
  #manage      
    nav
      font-size: 1rem
      .items
        display: inline-block
      ul li
        display: inline-block
        margin-right: 2vw
        &:last-child
          margin: 0
        a
          text-decoration: none
          transition: color .2s
    a.active
      color: #18BC9C       
</style>

