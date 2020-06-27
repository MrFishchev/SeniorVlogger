<template>
    <div class="wrapper" id="blog-post">
        <router-link class="category slide-in-right" :to="'/posts/category/' + categoryURL" v-if="data.category">
            {{ data.category.name || '' }}
        </router-link>
        <h1 class="title slide-in-right">{{data.title}}</h1>

        <hr />
        <div class="like slide-in-left">
            <h3 class="date" v-if="data.author" ><i>By {{ data.author.name}} on <span>{{ data.publishDate }}</span></i></h3>
            <div style="width:100px" v-if="isFBReady"
                class="fb-like"
                :data-href="postUrl"
                data-share="false"
                data-width="100px"
                data-layout="button_count"
                data-show-faces="true">
            </div>
        </div>

        <img :src="`/${data.imageUrl}`" class="image appers-fadein"/>

        <div id="content" class="appers-fadein" v-if="data.content" v-html="data.content" />

        <div class="additional-posts">
            <span v-if="next">Next:<router-link :to="'/posts/' + next.slug" :title="next.title">{{next.title}}</router-link></span>
            <span v-if="previous">Previous:<router-link :to="'/posts/' + previous.slug" :title="previous.title">{{previous.title}}</router-link></span>
        </div>
        
        <div class="tags">
            <span v-for="(tag, index) in data.tags"
                :key="index + tag">
                <router-link :to="'/posts/tag/' + tag">#{{tag}}</router-link>
            </span>
        </div>

        <!-- <div v-if="isFBReady" class="fb-comments" :data-href="postUrl" data-width="100%" data-numposts="10"></div> -->
    </div>
</template>

<script>
import 'highlight.js/scss/rainbow.scss'
const jQuery = require('jquery/dist/jquery.min.js')

export default {
    data (){
        return{
            isFBReady: false,
            next: null,
            previous: null,
            isLoading: true,
            data: {
                author: {},
                category: {}
            },
        }
    },

    async beforeMount() {
        let response = await this.$api.get(`/api/blog/slug/${this.$route.params.slug}`)
        this.data = response.data
        this.isLoading = false
    },

    computed: {
        categoryURL () {
            // return this.data.category.toLowerCase().trim()
        },
        postUrl(){
            // return `https://seniorvlogger.com/posts/${this.data.slug}` 
        }
    },

    beforeDestroy (){
        // window.removeEventListener('fb-sdk-ready', this.onFBReady)

        let blogLink = jQuery('#header a').eq(1)
        blogLink.removeClass("exact-active-route")
    },

    mounted () {
        let pre = jQuery('.content pre')
        if(pre){
            pre.each(function () {
                jQuery(this).wrapInner("<code class='hljs'></code>")
            })
        }

        let blogLink = jQuery('#header a').eq(1)
        blogLink.addClass("exact-active-route")

	
        // this.isFBReady = this.$fb != undefined
        // console.log(this.$fb)
        // if(this.$fb)
        // {
	    //     window.fbAsyncInit()
        // }
        // window.addEventListener('fb-sdk-ready', this.onFBReady)
    },

    methods: {
        // onFBReady: function () {
        //     this.isFBReady = true
        // }
    },
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
        max-width: 720px
        margin: 0 auto
        div
            width: 100%
            img
                max-width: 100%
                height: auto
        hr
            height: 1px
            width: 100%
            margin: 0 0 5px 0 
            color: $font-category
        .like
            display: flex
            justify-content: space-between
            width: 90%
        .category, .title
            text-transform: uppercase
            font-weight: 700
        .category
            color: $font-category
            font-size: 1.125rem
            text-decoration: none
            transition: color .1s
            letter-spacing: 1px
            line-height: 1.5
            &:hover
                color: $neon-light
        .title
            letter-spacing: 2px
            font-size: 2.5rem
            text-align: center
            margin-bottom: 1vw
        .date
            color: $font-date
            font-size: 1rem
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
        code
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
</style>
