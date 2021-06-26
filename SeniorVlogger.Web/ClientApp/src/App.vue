<template>
  <div id="app" class="main" data-theme="">
    <component :is="layout">
      <notifications group="app" class="notify" position="top right" />
      <router-view/>
    </component>
  </div>
</template>

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

<style lang="sass">
  #app.main, #app > .main
    min-height: 100vh
  .notify
    top: 100px!important
</style>
