<template>
  <div id="app" class="main">
    <component :is="layout">
      <router-view/>
    </component>
  </div>
</template>

<style lang="sass">
  @import "@/assets/style"
  #app.main, #app > .main
    min-height: 100vh
</style>

<script>
const default_layout = "default"

export default {
  computed: {
    layout() {
      let parent = this.$route.matched.filter(m => m.meta.layout)[0]
      let parentMeta = (parent != null) ? parent.meta.layout : null
      return (this.$route.meta.layout || parentMeta || default_layout) + '-layout'
    }
  }  
}
</script>
