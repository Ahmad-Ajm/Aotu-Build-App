# SpecKitForApp – n8n Orchestrator Guide

هذا المستند يشرح كيف يمكن تشغيل أوامر SpecKitForApp (clarify / plan / specify / tasks / analyze) من خلال n8n فوق مستودع GitHub وواجهة LLM عبر HTTP.


---

## 1. المدخلات العامة لأي Workflow

أي فلو في n8n يستدعي SpecKitForApp يجب أن يوفّر على الأقل:

- `repo` : مسار/اسم مستودع GitHub (مثال: `Ahmad-Ajm/Aotu-Build-App`).
- `branch` : الفرع المستهدف (مثال: `main`).
- `featureKey` : معرف الميزة (مثل `FEAT-QUESTIONS`).
- `command` : أحد الأوامر المعرفة في `specKitForApp/config/commands.json` (`clarify`, `plan`, `specify`, `tasks`, `analyze`).

اختياريًا (حسب نوع الأمر):

- `conversationFilePath` : مسار ملف سجل المحادثة في الريبو الذي يحوي حوار لغة الأعمال مع المستخدم.
- `featureDescription` : وصف حر للميزة (لن يُقرأ من ملف، بل من الـ payload) – مفيد لأمر `specify`.

---

## 2. قراءة إعدادات الأوامر (commands.json)

المسار: `specKitForApp/config/commands.json`

### خطوات في n8n

1. **قراءة الملف من GitHub**
   - عقدة HTTP Request (أو Git node) لقراءة محتوى `commands.json` من الفرع المحدد.
   - فك JSON إلى كائن n8n.

2. **اختيار تعريف الأمر**
   - من حقل `command` الوارد إلى الفلو، اختر:
     - `config.commands[command]`
   - إن لم يوجد الأمر، أعد خطأ واضحًا أو سجّل TODO.

3. **استخراج البيانات الأساسية**
   - `promptFile`
   - `outputPattern`
   - `inputContext[]`

> أي غموض في المسارات (مثل BRD أو الدستور) موثّق داخليًا في `commands.json` كـ `todo` ليتم حسمه من مالك المشروع.

---

## 3. جمع ملفات السياق من GitHub

باستخدام مصفوفة `inputContext` لكل أمر:

1. **تحويل قوالب المسارات**
   - استبدال `{featureKey}` بقيمة `featureKey` الحقيقية.
   - استبدال `{conversationFilePath}` بقيمة الحقل الوارد من الفلو (إن وُجد).

2. **تحميل الملفات**
   - لكل عنصر في `inputContext` له `pathTemplate` غير فارغ:
     - استخدم عقدة HTTP Request نحو GitHub API:
       - `GET /repos/{owner}/{repo}/contents/{path}?ref={branch}`
     - إذا كان `required: true` والملف غير موجود:
       - أعد خطأ واضحًا أو سجّل النقص في سجل الفلو (حسب ما يقرّره مالك المشروع).
   - خزّن محتوى الملفات (بعد فك Base64) في حقول مثل:
     - `context.currentSpec`
     - `context.currentPlan`
     - `context.currentTasks`
     - `context.featureMap`
     - `context.brd`
     - `context.conversationLog`

3. **Source بدون pathTemplate**
   - عناصر مثل `featureDescription` تأتي مباشرة من الـ payload، وليس من GitHub.

---

## 4. بناء البرومبت النهائي من ملف promptFile

1. **قراءة ملف البرومبت**
   - المسار: `promptFile` من config (مثال: `specKitForApp/prompts/clarify.md`).
   - قراءة المحتوى النصي من GitHub (الفرع المحدد).

2. **استبدال الـ placeholders**
   - اعمل replace بسيط على النص:
     - `{{featureKey}}` → قيمة `featureKey`.
     - `{{featureDescription}}` → نص الوصف الحر إن وجد.
     - `{{conversationLog}}` → محتوى conversation log أو نص فارغ إن لم يوجد.
     - `{{currentSpec}}`, `{{currentPlan}}`, `{{currentTasks}}`, `{{featureMap}}`, `{{brd}}`, `{{constitution}}`, `{{dataModel}}`, `{{contracts}}`, `{{researchDoc}}` حسب ما هو متوفر في `inputContext`.
   - يمكن أن يكون بعض هذه placeholders غير مستخدم في بعض الأوامر.

3. **تجهيز رسائل LLM**
   - أبسط شكل:
     - `messages = [{ role: "system", content: "<تعليمات عامة إذا أردت>" }, { role: "user", content: promptTextAfterReplacement }]`
   - أو يمكنك اعتبار كل البرومبت في رسالة واحدة `user` بدون system.

---

## 5. استدعاء واجهة LLM عبر HTTP

### مثال مبسط (نمط OpenAI-Compatible)

```json
POST https://api.openai.com/v1/chat/completions
Authorization: Bearer YOUR_API_KEY
Content-Type: application/json

{
  "model": "gpt-4.1-mini",
  "temperature": 0.3,
  "messages": [
    { "role": "system", "content": "You are SpecKitForApp agent..." },
    { "role": "user", "content": "<prompt-with-context-here>" }
  ]
}
```

### في n8n

- استخدم عقدة HTTP Request أو عقدة OpenAI/LLM جاهزة.
- عيّن `model` حسب `llmModelHint` في `commands.json` إذا رغبت.
- انتبه لمحدودية الـ tokens، خاصةً عندما تحمّل ثلاث ملفات (spec/plan/tasks).

---

## 6. كتابة المخرجات إلى GitHub

1. **قراءة `outputPattern`**
   - استبدل `{featureKey}` بالقيمة الفعلية للحصول على مسار الملف الهدف، مثلاً:
     - `specifications/11-features/FEAT-QUESTIONS/clarify.md`

2. **GitHub Write**
   - استخدم GitHub API:
     - `PUT /repos/{owner}/{repo}/contents/{path}`
   - المتطلبات:
     - `message` (نص commit)
     - `content` (Base64 لمحتوى Markdown الناتج من LLM)
     - `branch`
     - `sha` (فقط لو كان الملف موجودًا مسبقًا – يتم جلبه أولًا بـ GET)

3. **حالات خاصة**
   - إذا كان الأمر `analyze`:
     - يمكن اختيار:
       - إما كتابة تقرير إلى ملف (مثلاً `analysis.md` حسب `outputPattern`)،
       - أو إرجاع التقرير فقط إلى المستدعي (HTTP Response من n8n) بدون كتابة، حسب متطلباتك.

---

## 7. مثال فلو مصغّر لأمر clarify

1. Manual Trigger أو Webhook يستقبل:
   - `featureKey`
   - `conversationFilePath`
2. HTTP → GitHub: اقرأ `commands.json`.
3. Function Node: اختر `commands.clarify` واستخرج:
   - `promptFile`
   - `outputPattern`
   - `inputContext`
4. HTTP → GitHub: اقرأ ملفات السياق المطلوبة (conversation log, spec, featureMap, brd).
5. HTTP → GitHub: اقرأ `specKitForApp/prompts/clarify.md`.
6. Function Node: استبدل placeholders.
7. HTTP → LLM: أرسل البرومبت.
8. HTTP → GitHub: اكتب الناتج في مسار `outputPattern`.
9. (اختياري) أعد ملخصًا في Response من n8n (رابط الملف، حجم المحتوى، إلخ).

---

## 8. ربط SpecKitForApp مع صفحة HTML الحالية

الصفحة الحالية في هذا المشروع تقوم بـ:

- جمع مواصفات الميزة وسجلات المحادثة وإرسالها/حفظها في GitHub.

يمكن توسيعها لاحقًا لتقوم بـ:

- حفظ conversation log لكل ميزة في مسار موحّد (مثال: `specifications/11-features/{featureKey}/conversation.md`).
- استدعاء Webhook في n8n مع:
  - `featureKey`
  - `command` (مثل `clarify` أو `specify`)
  - `conversationFilePath`
- n8n عندها يستخدم SpecKitForApp كما هو موصوف أعلاه لتوليد:
  - `clarify.md`
  - `spec.md`
  - `plan.md`
  - `tasks.md`

---

## 9. أسئلة مفتوحة / TODO

- تحديد المسار النهائي الرسمي لـ BRD (حاليًا مستخدم: `specifications/10-brd/brd.md` – يحتاج تأكيد).
- تأكيد ما إذا كان `.specify/memory/constitution.md` هو دائمًا ملف الدستور المستهدف، أم يجب نسخ محتواه إلى مسار داخل `specifications/`.
- تحديد سياسة موحّدة لما إذا كان أمر `analyze`:
  - يكتب دائمًا تقريرًا إلى ملف،
  - أم يعيد التقرير فقط إلى المستدعي.

يمكن لمالك المشروع تعديل هذه النقاط في `commands.json` أو في هذا المستند عند اتخاذ القرار.


