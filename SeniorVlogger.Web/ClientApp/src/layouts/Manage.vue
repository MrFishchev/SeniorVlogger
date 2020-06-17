<template>
  <div id="manage">
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
      <router-link :to="{ path: '/manage'}" replace class="navbar-brand" exact-active-class="active">Manage</router-link>
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
            <router-link :to="{ path: '/manage/posts'}" replace class="nav-link" active-class="active">Posts</router-link>
          </li>
          <li class="nav-item">
            <router-link :to="{ path: '/manage/categories'}" replace class="nav-link" active-class="active">Categories</router-link>
          </li>
          <li class="nav-item">
            <router-link :to="{ path: '/manage/users'}" replace class="nav-link" active-class="active">Users</router-link>
          </li>
        </ul>

        <ul class="navbar-nav ml-auto mr-5">
          <li class="nav-item">
            <router-link to="about" class="nav-link">About</router-link>
          </li>
          <li class="nav-item">
            <router-link to="blog" class="nav-link">Blog</router-link>
          </li>
        </ul>
        <form class="form-inline my-2 my-lg-0 d-none d-lg-block">
          <input class="form-control mr-sm-2" type="text" placeholder="Search" />
          <button class="btn btn-secondary my-2 my-sm-0" type="submit">Search</button>
        </form>
      </div>
    </nav>

    <slot />
  </div>
</template>

<style lang="sass">
body
  background: #fff!important
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

