<template>
    <quillEditor class="quill-editor" 
        :content="buffer"
        @change="onChanged($event)"
        :options="editorOption">
    </quillEditor>
</template>

<script>
import 'highlight.js/scss/rainbow.scss'
import hljs from 'highlight.js'
import { quillEditor } from 'vue-quill-editor'
import Quill from 'quill'
import ImageResize from 'quill-image-resize-vue'
import 'quill/dist/quill.core.css'
import 'quill/dist/quill.snow.css'
Quill.register('modules/imageResize', ImageResize)

export default {
    components: {
        quillEditor
    },

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
                        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
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

<style lang="sass">
.quill-editor
    .ql-container
        overflow-y: scroll
        max-height: 800px
</style>