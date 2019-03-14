import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guard/auth.guard';
import { MemberDetailsComponent } from './member/member-details/member-details.component';
import { MemberDetailsResolver } from './_resolvers/member-details-resolver';
import { MemberListResolver } from './_resolvers/member-list-resolver';
import { MemberEditComponent } from './member/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit-resolver';
import { PreventUnsavedChanges } from './_guard/prevent-unsaved-changes.guard';
import { ListResolver } from './_resolvers/list-resolver';
import { MessagesResolver } from './_resolvers/messages-resolver';

export const appRoute: Routes = [
    {path: '', component : HomeComponent},
    {   path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {path: 'members', component : MemberListComponent, resolve: {users: MemberListResolver}},
            {path: 'members/:id', component : MemberDetailsComponent, resolve: {user: MemberDetailsResolver}},
            {path: 'member/edit', component : MemberEditComponent, resolve: {user: MemberEditResolver}
                , canDeactivate: [PreventUnsavedChanges]},
            {path: 'messages', component : MessagesComponent, resolve: {messages: MessagesResolver}},
            {path: 'lists', component : ListsComponent, resolve: {user: ListResolver}},
        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'},
]