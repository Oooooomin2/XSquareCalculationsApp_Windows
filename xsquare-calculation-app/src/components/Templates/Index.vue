<template>
  <div class="grid grid-cols-3 gap-4 mt-20 w-2/3 mx-auto huifont109">
    <div
      class="
        transition
        border-solid border-green-200
        rounded
        border
        p-7
        bg-white
        cursor-pointer
        transform
        hover:-translate-y-1 hover:scale-105
      "
      v-for="template in templates"
      :key="template.templateId"
      @click="toDetailPage(template)"
    >
      <img
        class="h-3/5 mx-auto"
        :src="'data:image/png;base64,' + template.thumbNail"
      />
      <div class="my-2 text-2xl">
        テンプレート名: {{ template.templateName }}
      </div>
      <div class="my-2 text-2xl">制作者: {{ template.userName }} 様</div>
      <div class="my-2 text-sm">作成日時: {{ template.createdTime }}</div>
      <div class="my-2 text-sm">更新日時: {{ template.updatedTime }}</div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      templates: [],
    };
  },
  methods: {
    toDetailPage(template) {
      this.$router.push({
        name: "template-detail",
        params: { id: template.templateId },
      });
    },
  },
  created() {
    let vm = this;
    axios
      .get("/template")
      .then(function (response) {
        response.data.forEach((element) => {
          console.log(element)
          vm.templates.push({
            templateId: element.Template.TemplateId,
            thumbNail: element.Template.ThumbNail,
            templateName: element.Template.TemplateName,
            userId: element.Template.UserId,
            userName: element.UserName,
            likeCount: element.Template.LikeCount,
            downloadCount: element.Template.DownloadCount,
            createdTime: element.Template.CreatedTime.replace("T", " "),
            updatedTime: element.Template.UpdatedTime.replace("T", " "),
          });
        });
      })
      .catch(function (error) {
        console.log(error);
      });
  },
};
</script>

<style scoped>
</style>