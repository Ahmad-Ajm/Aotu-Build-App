import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { CVService } from '../services/cv.service';
import { 
  CVDto, 
  CreateCVDto, 
  UpdateCVDto, 
  PersonalInfoDto, 
  ContactInfoDto,
  EducationDto,
  ExperienceDto,
  SkillDto,
  SkillLevel 
} from '../models/cv.model';

@Component({
  selector: 'app-cv-editor',
  templateUrl: './cv-editor.component.html',
  styleUrls: ['./cv-editor.component.css']
})
export class CVEditorComponent implements OnInit, OnDestroy {
  mode: 'create' | 'edit' | 'view' = 'create';
  cvId?: string;
  cv?: CVDto;
  loading = false;
  saving = false;
  autoSaveInterval?: any;
  
  cvForm: FormGroup;
  personalInfoForm: FormGroup;
  contactInfoForm: FormGroup;
  educationFormArray: FormArray;
  experienceFormArray: FormArray;
  skillsFormArray: FormArray;
  
  private routeSub?: Subscription;
  
  templates = [
    { value: 'professional', label: 'قالب احترافي' },
    { value: 'creative', label: 'قالب مبدع' },
    { value: 'simple', label: 'قالب بسيط' }
  ];
  
  skillLevels = [
    { value: SkillLevel.Beginner, label: 'مبتدئ' },
    { value: SkillLevel.Intermediate, label: 'متوسط' },
    { value: SkillLevel.Advanced, label: 'متقدم' },
    { value: SkillLevel.Expert, label: 'خبير' }
  ];
  
  employmentTypes = [
    { value: 'full-time', label: 'دوام كامل' },
    { value: 'part-time', label: 'دوام جزئي' },
    { value: 'contract', label: 'عقد' },
    { value: 'freelance', label: 'عمل حر' },
    { value: 'internship', label: 'تدريب' }
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private cvService: CVService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {
    // Initialize form groups
    this.personalInfoForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      address: [''],
      city: [''],
      country: [''],
      postalCode: [''],
      website: [''],
      linkedIn: [''],
      github: [''],
      twitter: [''],
      dateOfBirth: [''],
      nationality: ['']
    });
    
    this.contactInfoForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: [''],
      address: [''],
      city: [''],
      country: [''],
      postalCode: [''],
      website: [''],
      linkedIn: [''],
      github: [''],
      twitter: [''],
      dateOfBirth: [''],
      nationality: ['']
    });
    
    this.educationFormArray = this.fb.array([]);
    this.experienceFormArray = this.fb.array([]);
    this.skillsFormArray = this.fb.array([]);
    
    this.cvForm = this.fb.group({
      title: ['', Validators.required],
      template: ['professional', Validators.required],
      isPublic: [false],
      personalInfo: this.personalInfoForm,
      contactInfo: this.contactInfoForm,
      education: this.educationFormArray,
      experience: this.experienceFormArray,
      skills: this.skillsFormArray
    });
  }

  ngOnInit() {
    this.routeSub = this.route.data.subscribe(data => {
      const routeData = data as { mode?: 'create' | 'edit' | 'view' };
      this.mode = routeData.mode || 'create';
      
      if (this.mode === 'edit' || this.mode === 'view') {
        this.cvId = this.route.snapshot.params['id'];
        this.loadCV();
      }
      
      if (this.mode === 'view') {
        this.cvForm.disable();
      }
    });
    
    // Start auto-save for edit mode
    if (this.mode === 'edit') {
      this.startAutoSave();
    }
  }

  ngOnDestroy() {
    this.routeSub?.unsubscribe();
    this.stopAutoSave();
  }

  loadCV() {
    if (!this.cvId) return;
    
    this.loading = true;
    this.cvService.getCV(this.cvId).subscribe({
      next: (cv) => {
        this.cv = cv;
        this.populateForm(cv);
        this.loading = false;
      },
      error: (error) => {
        this.showError('حدث خطأ أثناء تحميل السيرة الذاتية');
        this.loading = false;
      }
    });
  }

  populateForm(cv: CVDto) {
    this.cvForm.patchValue({
      title: cv.title,
      template: cv.template,
      isPublic: cv.isPublic
    });
    
    // Parse and populate personal info
    if (cv.personalInfo) {
      try {
        const personalInfo = JSON.parse(cv.personalInfo);
        this.personalInfoForm.patchValue(personalInfo);
      } catch (e) {
        console.error('Error parsing personal info:', e);
      }
    }
    
    // Parse and populate education
    if (cv.education) {
      try {
        const education = JSON.parse(cv.education);
        this.educationFormArray.clear();
        education.forEach((edu: EducationDto) => {
          this.addEducation(edu);
        });
      } catch (e) {
        console.error('Error parsing education:', e);
      }
    }
    
    // Parse and populate experience
    if (cv.workExperience) {
      try {
        const experience = JSON.parse(cv.workExperience);
        this.experienceFormArray.clear();
        experience.forEach((exp: ExperienceDto) => {
          this.addExperience(exp);
        });
      } catch (e) {
        console.error('Error parsing experience:', e);
      }
    }
    
    // Parse and populate skills
    if (cv.skills) {
      try {
        const skills = JSON.parse(cv.skills);
        this.skillsFormArray.clear();
        skills.forEach((skill: SkillDto) => {
          this.addSkill(skill);
        });
      } catch (e) {
        console.error('Error parsing skills:', e);
      }
    }
  }

  createEducationForm(edu?: EducationDto): FormGroup {
    return this.fb.group({
      degree: [edu?.degree || '', Validators.required],
      institution: [edu?.institution || '', Validators.required],
      fieldOfStudy: [edu?.fieldOfStudy || ''],
      startDate: [edu?.startDate || '', Validators.required],
      endDate: [edu?.endDate || ''],
      isCurrentlyStudying: [edu?.isCurrentlyStudying || false],
      gpa: [edu?.gpa || ''],
      gpaScale: [edu?.gpaScale || ''],
      description: [edu?.description || ''],
      location: [edu?.location || ''],
      order: [edu?.order || 0]
    });
  }

  createExperienceForm(exp?: ExperienceDto): FormGroup {
    return this.fb.group({
      jobTitle: [exp?.jobTitle || '', Validators.required],
      company: [exp?.company || '', Validators.required],
      location: [exp?.location || ''],
      startDate: [exp?.startDate || '', Validators.required],
      endDate: [exp?.endDate || ''],
      isCurrentlyWorking: [exp?.isCurrentlyWorking || false],
      employmentType: [exp?.employmentType || 'full-time'],
      industry: [exp?.industry || ''],
      description: [exp?.description || ''],
      achievements: [exp?.achievements || ''],
      skillsUsed: [exp?.skillsUsed || ''],
      order: [exp?.order || 0]
    });
  }

  createSkillForm(skill?: SkillDto): FormGroup {
    return this.fb.group({
      name: [skill?.name || '', Validators.required],
      level: [skill?.level || SkillLevel.Beginner],
      yearsOfExperience: [skill?.yearsOfExperience || ''],
      category: [skill?.category || ''],
      description: [skill?.description || ''],
      isFeatured: [skill?.isFeatured || false],
      order: [skill?.order || 0],
      lastUsed: [skill?.lastUsed || '']
    });
  }

  addEducation(edu?: EducationDto) {
    this.educationFormArray.push(this.createEducationForm(edu));
  }

  addExperience(exp?: ExperienceDto) {
    this.experienceFormArray.push(this.createExperienceForm(exp));
  }

  addSkill(skill?: SkillDto) {
    this.skillsFormArray.push(this.createSkillForm(skill));
  }

  removeEducation(index: number) {
    this.educationFormArray.removeAt(index);
  }

  removeExperience(index: number) {
    this.experienceFormArray.removeAt(index);
  }

  removeSkill(index: number) {
    this.skillsFormArray.removeAt(index);
  }

  moveEducationUp(index: number) {
    if (index > 0) {
      const current = this.educationFormArray.at(index);
      const previous = this.educationFormArray.at(index - 1);
      this.educationFormArray.setControl(index - 1, current);
      this.educationFormArray.setControl(index, previous);
    }
  }

  moveEducationDown(index: number) {
    if (index < this.educationFormArray.length - 1) {
      const current = this.educationFormArray.at(index);
      const next = this.educationFormArray.at(index + 1);
      this.educationFormArray.setControl(index + 1, current);
      this.educationFormArray.setControl(index, next);
    }
  }

  moveExperienceUp(index: number) {
    if (index > 0) {
      const current = this.experienceFormArray.at(index);
      const previous = this.experienceFormArray.at(index - 1);
      this.experienceFormArray.setControl(index - 1, current);
      this.experienceFormArray.setControl(index, previous);
    }
  }

  moveExperienceDown(index: number) {
    if (index < this.experienceFormArray.length - 1) {
      const current = this.experienceFormArray.at(index);
      const next = this.experienceFormArray.at(index + 1);
      this.experienceFormArray.setControl(index + 1, current);
      this.experienceFormArray.setControl(index, next);
    }
  }

  saveCV() {
    if (this.cvForm.invalid) {
      this.showError('يرجى ملء جميع الحقول المطلوبة');
      return;
    }

    this.saving = true;
    
    const cvData = this.prepareCVData();
    
    if (this.mode === 'create') {
      this.createCV(cvData);
    } else if (this.mode === 'edit' && this.cvId) {
      this.updateCV(cvData);
    }
  }

  prepareCVData(): any {
    const formValue = this.cvForm.value;
    
    return {
      title: formValue.title,
      template: formValue.template,
      isPublic: formValue.isPublic,
      personalInfo: JSON.stringify(formValue.personalInfo),
      contactInfo: JSON.stringify(formValue.contactInfo),
      education: JSON.stringify(formValue.education),
      workExperience: JSON.stringify(formValue.experience),
      skills: JSON.stringify(formValue.skills)
    };
  }

  createCV(cvData: CreateCVDto) {
    this.cvService.createCV(cvData).subscribe({
      next: (createdCV) => {
        this.showSuccess('تم إنشاء السيرة الذاتية بنجاح');
        this.router.navigate(['/cv/edit', createdCV.id]);
      },
      error: (error) => {
        this.showError('حدث خطأ أثناء إنشاء السيرة الذاتية');
        this.saving = false;
      }
    });
  }

  updateCV(cvData: UpdateCVDto) {
    if (!this.cvId) return;
    
    this.cvService.updateCV(this.cvId, cvData).subscribe({
      next: (updatedCV) => {
        this.cv = updatedCV;
        this.showSuccess('تم حفظ التغييرات بنجاح');
        this.saving = false;
      },
      error: (error) => {
        this.showError('حدث خطأ أثناء حفظ التغييرات');
        this.saving = false;
      }
    });
  }

  startAutoSave() {
    // Auto-save every 30 seconds
    this.autoSaveInterval = setInterval(() => {
      if (this.cvForm.dirty && this.cvForm.valid && this.mode === 'edit' && this.cvId) {
        const cvData = this.prepareCVData();
        this.cvService.updateCV(this.cvId, cvData).subscribe({
          next: () => {
            console.log('Auto-save completed');
          },
          error: (error) => {
            console.error('Auto-save failed:', error);
          }
        });
      }
    }, 30000);
  }

  stopAutoSave() {
    if (this.autoSaveInterval) {
      clearInterval(this.autoSaveInterval);
    }
  }

  exportToPDF() {
    if (!this.cvId) return;
    
    const options = {
      template: this.cvForm.value.template,
      language: 'ar',
      includePhoto: true,
      includeSensitiveInfo: false
    };
    
    this.cvService.exportToPdf(this.cvId, options).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${this.cvForm.value.title}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        this.showError('حدث خطأ أثناء التصدير إلى PDF');
      }
    });
  }

  exportToWord() {
    if (!this.cvId) return;
    
    const options = {
      template: this.cvForm.value.template,
      language: 'ar',
      includePhoto: true,
      includeSensitiveInfo: false
    };
    
    this.cvService.exportToWord(this.cvId, options).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${this.cvForm.value.title}.docx`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        this.showError('حدث خطأ أثناء التصدير إلى Word');
      }
    });
  }

  toggleVisibility() {
    if (!this.cvId) return;
    
    this.cvService.toggleCVVisibility(this.cvId).subscribe({
      next: (updatedCV) => {
        this.cv = updatedCV;
        this.cvForm.patchValue({ isPublic: updatedCV.isPublic });
        this.showSuccess(`تم ${updatedCV.isPublic ? 'جعل السيرة الذاتية عامة' : 'جعل السيرة الذاتية خاصة'}`);
      },
      error: (error) => {
        this.showError('حدث خطأ أثناء تغيير حالة الرؤية');
      }
    });
  }

  showSuccess(message: string) {
    this.snackBar.open(message, 'إغلاق', {
      duration: 3000,
      horizontalPosition: 'left',
      verticalPosition: 'bottom'
    });
  }

  showError(message: string) {
    this.snackBar.open(message, 'إغلاق', {
      duration: 5000,
      horizontalPosition: 'left',
      verticalPosition: 'bottom',
      panelClass: ['error-snackbar']
    });
  }

  get educationControls() {
    return this.educationFormArray.controls;
  }

  get experienceControls() {
    return this.experienceFormArray.controls;
  }

  get skillsControls() {
    return this.skillsFormArray.controls;
  }
}