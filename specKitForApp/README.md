# SpecKitForApp – Spec Kit Orchestrated via GitHub + n8n + HTTP LLM

هذا المجلد يحتوي على نسخة مستقلة من SpecKit مصممة للعمل فوق:

- مستودع GitHub كمصدر الحقيقة الوحيد (BRD, specs, feature folders, logs)
- n8n كأوركستريتور (workflow engine)
- واجهة HTTP لأي LLM (OpenAI, DeepSeek, Gemini, …)

## المكونات الرئيسية

- `config/commands.json`  
  تعريف مركزي لكل الأوامر (clarify / plan / specify / tasks / analyze…) مع:
  - ملف البرومبت (`promptFile`)
  - نمط مسار المخرجات (`outputPattern`) باستخدام `{featureKey}`
  - مصادر السياق المطلوب قراءتها من GitHub (`inputContext`)

- `prompts/*.md`  
  قوالب برومبت جاهزة لكل أمر. n8n يقوم بحقن نصوص الملفات (conversation log, spec, plan, tasks…) مكان الـ placeholders مثل:
  - `{{featureKey}}`
  - `{{conversationLog}}`
  - `{{currentSpec}}`

- `templates/*.md`  
  قوالب توثيقية توضح البنية المتوقعة للمخرجات (clarify.md, plan.md, spec.md, tasks.md). هذه القوالب مرجع للبشر وللـ LLM، لكنها ليست البرومبتات نفسها.

- `docs/n8n-orchestrator.md`  
  توثيق لكيفية تشغيل SpecKitForApp من خلال n8n:
  - ما هي الـ inputs (repo, branch, featureKey, conversationFilePath, …)
  - كيف يقرأ `config/commands.json`
  - كيف يحمّل ملفات السياق من GitHub
  - كيف يبني طلب HTTP للـ LLM
  - أين يكتب المخرجات في الريبو

> ملاحظة: ملفات SpecKit الأصلية داخل `.cursor/commands/speckit.*` و`.specify/*` تبقى كما هي للاستخدام داخل Cursor فقط، وتُستخدم هنا كمرجع تصميمي لا أكثر.


