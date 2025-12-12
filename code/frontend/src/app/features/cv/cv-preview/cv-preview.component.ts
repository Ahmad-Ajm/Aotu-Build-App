import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CVDto, PersonalInfoDto, EducationDto, ExperienceDto, SkillDto, SkillLevel } from '../models/cv.model';

@Component({
  selector: 'app-cv-preview',
  templateUrl: './cv-preview.component.html',
  styleUrls: ['./cv-preview.component.css']
})
export class CVPreviewComponent implements OnChanges {
  @Input() cvData: CVDto | null = null;
  @Input() template: string = 'professional';
  @Input() language: string = 'ar';
  
  personalInfo: PersonalInfoDto | null = null;
  education: EducationDto[] = [];
  experience: ExperienceDto[] = [];
  skills: SkillDto[] = [];
  
  skillLevelLabels = {
    [SkillLevel.Beginner]: 'مبتدئ',
    [SkillLevel.Intermediate]: 'متوسط',
    [SkillLevel.Advanced]: 'متقدم',
    [SkillLevel.Expert]: 'خبير'
  };

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['cvData'] && this.cvData) {
      this.parseCVData();
    }
  }

  private parseCVData(): void {
    if (!this.cvData) return;

    try {
      // Parse personal info
      if (this.cvData.personalInfo) {
        this.personalInfo = typeof this.cvData.personalInfo === 'string' 
          ? JSON.parse(this.cvData.personalInfo)
          : this.cvData.personalInfo;
      }

      // Parse education
      if (this.cvData.education) {
        const educationData = typeof this.cvData.education === 'string'
          ? JSON.parse(this.cvData.education)
          : this.cvData.education;
        this.education = Array.isArray(educationData) ? educationData : [];
      }

      // Parse experience
      if (this.cvData.workExperience) {
        const experienceData = typeof this.cvData.workExperience === 'string'
          ? JSON.parse(this.cvData.workExperience)
          : this.cvData.workExperience;
        this.experience = Array.isArray(experienceData) ? experienceData : [];
      }

      // Parse skills
      if (this.cvData.skills) {
        const skillsData = typeof this.cvData.skills === 'string'
          ? JSON.parse(this.cvData.skills)
          : this.cvData.skills;
        this.skills = Array.isArray(skillsData) ? skillsData : [];
      }
    } catch (error) {
      console.error('Error parsing CV data:', error);
    }
  }

  getSkillLevelLabel(level: SkillLevel): string {
    return this.skillLevelLabels[level] || level;
  }

  formatDate(dateString: string | Date): string {
    if (!dateString) return 'حتى الآن';
    
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    
    return `${year}-${month.toString().padStart(2, '0')}`;
  }

  getDuration(startDate: string | Date, endDate: string | Date | null): string {
    const start = new Date(startDate);
    const end = endDate ? new Date(endDate) : new Date();
    
    const years = end.getFullYear() - start.getFullYear();
    const months = end.getMonth() - start.getMonth();
    
    let duration = '';
    if (years > 0) {
      duration += `${years} سنة`;
    }
    if (months > 0) {
      if (duration) duration += ' و ';
      duration += `${months} شهر`;
    }
    
    return duration || 'أقل من شهر';
  }

  getFeaturedSkills(): SkillDto[] {
    return this.skills.filter(skill => skill.isFeatured).slice(0, 5);
  }

  getOtherSkills(): SkillDto[] {
    return this.skills.filter(skill => !skill.isFeatured);
  }

  sortByOrder(items: any[]): any[] {
    return [...items].sort((a, b) => (a.order || 0) - (b.order || 0));
  }
}