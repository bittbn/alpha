import { Component, OnInit } from '@angular/core';
import {UserService} from '../../../services/user.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {UserModel} from '../models/user-model';
import {AddUserModel} from '../models/add-user-model';
import {UserShortModel} from '../models/user-short-model';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  form: FormGroup;
  actionType: string;
  formTitle: string;
  formBody: string;
  userId: string;
  errorMessage: any;
  existingUser: UserModel;

  constructor(private userService: UserService, private formBuilder: FormBuilder,
              private avRoute: ActivatedRoute, private router: Router) {
    const idParam = 'id';
    this.actionType = 'Add';
    this.formTitle = 'title';
    this.formBody = 'body';
    if (this.avRoute.snapshot.params[idParam]) {
      this.userId = this.avRoute.snapshot.params[idParam];
    }

    this.form = this.formBuilder.group(
      {
        postId: 0,
        title: ['', [Validators.required]],
        body: ['', [Validators.required]],
      }
    );
  }

  ngOnInit() {
    if (this.userId) {
      this.actionType = 'Edit';
      this.userService.getUser(this.userId)
        .subscribe(data => (
          this.existingUser = data,
            this.form.controls[this.formTitle].setValue(data.login),
            this.form.controls[this.formBody].setValue(data.name)
        ));
    }
  }

  save() {
    if (!this.form.valid) {
      return;
    }

    if (this.actionType === 'Add') {
      const user: AddUserModel = {
        password: '',
        roleId: 1,
        surname: '',
        login: this.form.get(this.formTitle).value,
        name: this.form.get(this.formBody).value
      };

      this.userService.addUser(user)
        .subscribe((data) => {
          this.router.navigate(['edit-user', data.id]);
        });
    }

    /*if (this.actionType === 'Edit') {
      let user: UserShortModel = {
        postId: this.existingBlogPost.postId,
        dt: this.existingBlogPost.dt,
        creator: this.existingBlogPost.creator,
        title: this.form.get(this.formTitle).value,
        body: this.form.get(this.formBody).value
      };
      this.blogPostService.updateBlogPost(blogPost.postId, blogPost)
        .subscribe((data) => {
          this.router.navigate([this.router.url]);
        });
    }*/
  }

  cancel() {
    this.router.navigate(['/']);
  }

  get title() { return this.form.get(this.formTitle); }
  get body() { return this.form.get(this.formBody); }
}
