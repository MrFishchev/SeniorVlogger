<template>
    <div class="wrapper" id="blog-post">
        <router-link class="category slide-in-right" :to="'/posts/category/' + categoryURL" v-if="data.category">
            {{ data.category.name || '' }}
        </router-link>
        <h1 class="title slide-in-right">{{data.title}}</h1>

        <hr />
        <div class="like slide-in-left">
            <h2 class="date">{{ createdDate }}</h2>
            <div style="width:100px" v-if="!edit"
                class="fb-like"
                :data-href="postUrl"
                data-share="false"
                data-width="100px"
                data-layout="button_count"
                data-show-faces="true">
            </div>
        </div>

        <img v-if="!edit" :src="`/${data.imageUrl}`" class="image appers-fadein"/>

        <div id="content" class="appers-fadein" v-if="data.content" v-html="data.content" />

        <div class="additional-posts">
            <span v-if="data.next && data.next.slug">Next part:<router-link :to="'/blog/' + data.next.slug">{{data.next.title}}</router-link></span>
            <span v-if="data.previous && data.previous.slug">Previous part:<router-link :to="'/blog/' + data.previous.slug">{{data.previous.title}}</router-link></span>
        </div>
        
        <div class="tags">
            <span v-for="(tag, index) in data.tags"
                :key="index + tag">
                <router-link :to="'/blog/tag/' + tag">#{{tag}}</router-link>
            </span>
        </div>

        
        <div v-if="!edit && isLightMode" class="fb-comments" :data-href="postUrl" data-order-by="reverse_time" data-colorscheme="dark"
             data-width="100%" data-numposts="10" ></div>
        <div v-if="!edit && !isLightMode" class="fb-dark-error">
          <p>Unfortunately, Facebook Comments plugin is not readable in the dark mode until fix by facebook team.</p>
        </div>
    </div>
</template>

<script>
import 'highlight.js/scss/rainbow.scss'
import hljs from 'highlight.js'
const jQuery = require('jquery/dist/jquery.min.js')

export default {
    props: {
        edit: {
            type: Boolean,
            default: false
        },

        editData: {
            type: Object,
            default: null
        }
    },

    data (){
        return{
            fbIsReady: false,
            data: {
                author: {},
                category: {}
            },
        }
    },

    watch: {
        editData(val) {
            this.data = val
        },

        "$route.params.slug"(val) {
            this.LoadData()
        }
    },

    beforeMount() {
        if(this.edit) return
        window.addEventListener('fb-sdk-ready', () => this.fbIsReady = true)
        this.LoadData()
    },

    methods: {
        LoadData() {
            this.$api.get(`/api/blog/slug/${this.$route.params.slug}`)
                .then(response => {
                    this.data = response.data
                })
                .catch(error => {
                    this.$router.push('/404')
                })
        }
    },

    computed: {
        categoryURL () {
            // return this.data.category.toLowerCase().trim()
        },
        postUrl(){
            // return `https://seniorvlogger.com/posts/${this.data.slug}` 
        },
        createdDate() {
            if(!this.data.publishDate) return
            
            return this.data.publishDate.split(' ')[0]
        },
      
        isLightMode(){
          return this.$store.getters.IsLightMode
        }
    },

    beforeDestroy (){
        window.removeEventListener('fb-sdk-ready', this.onFBReady)

        let blogLink = jQuery('#header a').eq(1)
        blogLink.removeClass("exact-active-route")
    },

    mounted () {
        if(!this.fbIsReady && window.FB)
            window.fbAsyncInit()
    },

    updated() {
        document.querySelectorAll('.ql-syntax').forEach((block) => {
            hljs.highlightBlock(block);
        }); 

        let blogLink = jQuery('#header a').eq(1)
        blogLink.addClass("exact-active-route")
    }
}
</script>

<style lang="sass">
    @import '@/assets/_blog'
    #blog-post
        padding: 3vw 0
        display: flex
        flex-direction: column
        align-items: center
        width: 100%
        max-width: 1080px
        margin: 0 auto
        .fb-dark-error
          color: #b3adb6
          font-style: italic
        #content
          color: var(--font-blog)
          .hljs
            background: var(--hljs-backgound)
        p.ql-align-center
            text-align: center
        div
            width: 100%
            img
                max-width: 100%
                height: auto
                cursor: default!important
        hr
            height: 1px
            width: 100%
            margin: 0 0 5px 0 
            color: var(--font-category)
        .like
            display: flex
            justify-content: space-between
            width: 90%
        .category, .title
            text-transform: uppercase
            font-weight: 700
        .category
            color: var(--font-category)
            font-size: 1.125rem
            text-decoration: none
            transition: color .1s
            letter-spacing: 1px
            line-height: 1.5
            &:hover
                color: var(--neon-light)
        .title
            color: var(--font-title)
            letter-spacing: 2px
            font-size: 2.5rem
            text-align: center
            margin-bottom: 1vw
        .date
            color: var(--font-date)
            font-size: 1.125rem
            font-weight: 400
            span
                font-weight: 600
        .image
            margin: 3vw 0
            width: 100%
            height: auto
            max-width: 700px
            max-height: 400px
            border-radius: 5px
        pre.ql-syntax
            padding: 20px
            max-height: 850px
            overflow: auto
        .fb-comments > iframe
            width: 100%
        .additional-posts
            font-weight: 600
            font-style: italic
            margin-bottom: 1vw
            padding: 0 5px
            span
                display: block
            a
                margin-left: 10px
        .tags
            margin-bottom: 3vw 
            span
                background: #f2f2f2
                padding: 3px   
                border-radius: 3px 
                transition: background-color .2s
                margin-right: 10px
                &:hover
                    background: #e0e0e0
                a
                    text-decoration: none
                    color: #595959  
        code
            border-radius: 5px
</style>
