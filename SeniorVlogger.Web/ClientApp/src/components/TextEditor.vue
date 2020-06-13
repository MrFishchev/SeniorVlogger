<template>
    <client-only placeholder="Loading...">
        <div class="quill-editor" 
            :content="buffer"
            @change="onChanged($event)"
            :options="editorOption"
            v-quill:myQuillEditor="editorOption">
        </div>
    </client-only>
</template>

<script>
import hljs from 'highlight.js/lib/highlight.js'
if(process.browser){
    require('~/plugins/nuxt-quill-plugin.js')
}

export default {
    props:{
        buffer: String
    },
    methods: {
        onChanged(event) {
            this.$emit('changed', event.html)
        }
    },

    data (){
        return{
            editorOption: {
                modules: {
                    imageResize: true,
                    toolbar: [
                        ['bold', 'italic'],
                        ['blockquote', 'code-block'],
                        [{ 'header': 1 }, { 'header': 2 }],
                        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                        [{ 'size': ['small', false, 'large', 'huge'] }],
                        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                        [{ 'font': [] }],
                        [{ 'color': [] }, { 'background': [] }],
                        [{ 'align': [] }],
                        ['clean'],
                        ['link', 'image']
                    ],
                    syntax: {
                        highlight: text => hljs.highlightAuto(text).value
                    }
                }
            }
        }
    }
   
}
</script>