<template>
  <div class="wrapper" v-if="!loading">

    <p v-if="!posts || posts.length==0" style="min-height:75vh;">There are not posts.</p>
    <BlogPostShort v-else v-for="post in posts"
      :key="post.id"
      :data="post" />

  </div>
</template>

<script>
import BlogPostShort from '@/components/BlogPostShort'

export default {
  components: {
    BlogPostShort
  },

  data (){
    return{
      loading: false,
      posts: []
    }
  },

  async beforeMount() {
    let response = await this.$api.get('/api/blog')
    this.posts = response.data
  }

}
</script>

<style lang="sass">
  #post-short:last-child 
    margin-bottom: 30px
    .separator
      display: none
</style>
