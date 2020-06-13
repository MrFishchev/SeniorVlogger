<template>
  <div id="app" class="main">
    <component :is="layout">
      <router-view/>
    </component>
  </div>
</template>

<style lang="sass">
@import "@/assets/style"
</style>

<script>
const default_layout = "default"

export default {
  computed: {
    layout() {
      return (this.$route.meta.layout || default_layout) + '-layout'
    }
  }  
}
</script>
