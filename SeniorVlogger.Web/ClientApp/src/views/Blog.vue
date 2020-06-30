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

  watch: {
     "$route.params"() {
        this.LoadData()
      }
  },

  methods: {
    async LoadData() {
      let response
      if(this.$route.params.category){
        response = await this.$api.get(`/api/blog/category/${this.$route.params.category}`)
      }
      else if(this.$route.params.tag){
        response = await this.$api.get(`/api/blog/tag/${this.$route.params.tag}`)
      }
      else{
        response = await this.$api.get('/api/blog')
      }

      this.posts = response.data
    }
  },

  beforeMount() {
    this.LoadData()
  }

}
</script>

<style lang="sass">
  #post-short:last-child 
    margin-bottom: 30px
    .separator
      display: none
</style>
