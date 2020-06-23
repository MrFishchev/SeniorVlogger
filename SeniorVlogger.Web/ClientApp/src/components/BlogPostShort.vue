<template>
    <div id="post-short">
        <router-link class="category slide-in-left" :to="'/blog/category/' + data.category.id">
            {{ data.category.name }}
        </router-link>

        <h1 class="title slide-in-right">
            <router-link :to="`/blog/${data.slug}`">{{ data.title }}</router-link>
        </h1>

        <h3 class="date slide-in-left">{{ data.date }}</h3>

        <router-link class="image appers-fadein" :to="`/blog/${data.slug}`">
            <img :src="`/${data.imageUrl}`" />
        </router-link>

        <p class="short appers-fadein">{{ data.description.substring(0, 195) }}...</p>
        <router-link class="more slide-in-right" :to="`/blog/${data.slug}`" :title="data.title">Read more</router-link>

        <hr class="separator"/>
    </div>
</template>

<script>
export default {
    props: {
        data: {
            type: Object,
            default: null
        }
    }
}
</script>

<style lang="sass">
    @import '@/assets/_blog'
    #post-short
        display: flex
        flex-direction: column
        align-items: center
        width: 100%
        padding: 2vw 0
        color: $font
        max-width: 720px
        margin: 0 auto
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
            a
                color: $font-title
                text-decoration: none
                &:hover
                    color: $font-title
        .date
            color: $font-date
            font-size: 1rem
            font-weight: 600
        .image
            background: $font-date
            margin: 2vw 0
            display: block
            width: 100%
            max-width: 700px
            max-height: 400px
            img
                max-width: 700px
                max-height: 400px
                width: 100%
                height: auto
                border-radius: 5px
        .short
            font-size: 1rem
            font-weight: 400
            color: $font
            text-align: center
        .more
            margin-bottom: 3vw
        .separator
            color: $font-date
            height: 2px
            width: 100%
</style>